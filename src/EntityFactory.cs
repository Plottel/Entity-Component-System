using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the class where all Entities are created. As Entities are just integers, this class and its methods
    /// provide templates for creating different Entity "types".
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        /// The World which any created Entities will be added to.
        /// </summary>
        private static World _world;

        /// <summary>
        /// Gets or sets the World.
        /// </summary>
        /// <value>The world.</value>
        public static World World
        {
            get {return _world;}
            set {_world = value;}
        }

        /// <summary>
        /// Returns a Bitmap representing the Castle to be used in the game.
        /// </summary>
        public static Bitmap CreateCastleImg()
        {
            Bitmap result = SwinGame.CreateBitmap(100, 590);
            SwinGame.ClearSurface(result, SwinGame.RGBAColor(50, 50, 40, 140));

            return result;
        }

        /// <summary>
        /// Returns a Bitmap representing a Freezing Bullet.
        /// </summary>
        /// <returns>The freezing bullet image.</returns>
        public static Bitmap CreateFreezingBulletImg()
        {
            Bitmap result = SwinGame.CreateBitmap(15, 15);
            SwinGame.ClearSurface(result, Color.MediumAquamarine);

            return result;
        }

        /// <summary>
        /// Creates an Entity with all Components of a Player and adds it to the World.
        /// </summary>
        public static void CreatePlayer()
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateCastleImg()));
            components.Add(new CPosition(0, 5, 100, 590));
            components.Add(new CPlayer());
            components.Add(new CHealth(200));
            components.Add(new CPlayerTeam());

            _world.AddEntity(newEntity, components);           
        }

        /// <summary>
        /// Creates an Entity with all Components of a Sword Man and adds it to the World.
        /// This represents the unit holding a sword in the game which attacks the Player's Castle.
        /// </summary>
        /// <param name="x">The x coordinate where the Entity will be created.</param>
        /// <param name="y">The y coordinate where the Entity will be created.</param>
        public static void CreateSwordMan(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Used by Velocity Component
            int speed = 2;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 21, 30));
            components.Add(new CVelocity(-speed, 0, speed)); //Is created moving towards the Castle.
            components.Add(new CHealth(3));
            components.Add(new CAI(1, 1000, AttackType.Melee, 1)); //Sword Men are on Enemy team and target the Castle by default
            components.Add(new CDamage(1));
            components.Add(new CLoot(5));
            components.Add(new CEnemyTeam());
            components.Add(new CStatusAnimations());

            Bitmap bmp = SwinGame.BitmapNamed("SwordMan");
            Animation anim = SwinGame.CreateAnimation("Walk", SwinGame.AnimationScriptNamed("SwordManAnims"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("SwordManAnims");

            components.Add(new CAnimation(bmp, anim, animScript));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components of an Archer for the Player team and adds it to the World.
        /// This represents the unit holding a bow which the Player can buy to defend the Castle.
        /// </summary>
        /// <param name="x">The x coordinate where the Entity will be created.</param>
        /// <param name="y">The y coordinate where the Entity will be created.</param>
        public static void CreatePlayerArcher(float x, float y)
        {
            //CreateEntity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 20, 28));
            components.Add(new CAI(700, 2000, AttackType.Gun));
            components.Add(new CGun(25, 2));
            components.Add(new CPlayerTeam());

            Bitmap bmp = SwinGame.BitmapNamed("PlayerArcher");
            Animation anim = SwinGame.CreateAnimation("Still", SwinGame.AnimationScriptNamed("ArcherManAnims"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("ArcherManAnims");

            components.Add(new CAnimation(bmp, anim, animScript));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components of an Archer for the Enemy team and adds it to the World.
        /// This represents the unit holding a bow which attacks the Player's Castle.
        /// </summary>
        /// <param name="x">The x coordinate where the Entity will be created.</param>
        /// <param name="y">The y coordinate where the Entity will be created.</param>
        public static void CreateEnemyArcher(float x, float y)
        {
            //Create Entity and add to world.
            Entity newEntity = _world.CreateEntity();
           
            //Used by Velocity Component.
            int speed = 5;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 20, 28));
            components.Add(new CVelocity(-speed, 0, speed)); //Is created moving towards the castle.
            components.Add(new CHealth(3));
            components.Add(new CAI(100, 2000, AttackType.Gun, 1)); //Belongs to the Enemy team - attacks the Castle by default.
            components.Add(new CGun(5, 2));
            components.Add(new CLoot(10));
            components.Add(new CEnemyTeam());
            components.Add(new CStatusAnimations());
                
            Bitmap bmp = SwinGame.BitmapNamed("ArcherMan");
            Animation anim = SwinGame.CreateAnimation("Walk", SwinGame.AnimationScriptNamed("ArcherManAnims"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("ArcherManAnims");

            components.Add(new CAnimation(bmp, anim, animScript));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components of a Battering Ram unit for the Enemy team and adds it to the World.
        /// The unit represents the large units holding Battering Rams which attack the Player's Castle.
        /// </summary>
        /// <param name="x">The x coordinate where the Entity will be created.</param>
        /// <param name="y">The y coordinate where the Entity will be created.</param>
        public static void CreateBatteringRam(float x, float y)
        {
            //Create Entity and add to world.
            Entity newEntity = _world.CreateEntity();

            //Used by Velocity Component.
            int speed = 5;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, 64, 58));
            components.Add(new CVelocity(-speed, 0, speed));
            components.Add(new CHealth(3));
            components.Add(new CAI(1, 5000, AttackType.Melee, 1)); //Belongs to the Enemy team - targets the Castle by default
            components.Add(new CDamage(5));
            components.Add(new CLoot(20));
            components.Add(new CEnemyTeam());
            components.Add(new CStatusAnimations());

            Bitmap bmp = SwinGame.BitmapNamed("BatteringRam");
            Animation anim = SwinGame.CreateAnimation("Walk", SwinGame.AnimationScriptNamed("BatteringRamAnims"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("BatteringRamAnims");

            components.Add(new CAnimation(bmp, anim, animScript));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components of an Arrow and adds it to the World.
        /// This Entity represents the Arrows used by Archers of both teams.
        /// </summary>
        /// <param name="x">The x coordinate where the Entity will be created.</param>
        /// <param name="y">The y coordinate where the Entity will be created.</param>
        /// <param name="speed">The speed of the Arrow.</param>
        /// <param name="damage">The amount of damage the Arrow will inflict on impact.</param>
        /// <param name="target">The position the Arrow is heading towards.</param>
        /// <param name="team">The team the Arrow belongs to</param>
        public static void CreateArrow(float x, float y, int speed, int damage, CPosition target, string team)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();

            float xOffset = (target.X - x);
            float yOffset = (target.Y - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * speed;
            float yVel = (yOffset / vectorLength) * speed;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(new CPosition(target.X - 5, target.Y - 5, 10, 10)));
            components.Add(new CPosition(x, y, 15));
            components.Add(new CDamage(damage));
            components.Add(new CCollidable());
            components.Add(new CArrow());

            if (team == "Player")
            {
                components.Add(new CPlayerTeam());
                components.Add(new CRenderable(SwinGame.BitmapNamed("PlayerArrow")));
            }
            else
            {
                components.Add(new CEnemyTeam());
                components.Add(new CRenderable(SwinGame.BitmapNamed("Arrow")));
            }

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components for a Freezing Bullet and adds it to the World.
        /// This Entity represents the Freezing Bullets the Player can spawn to Freeze units.
        /// </summary>
        /// <param name="x">The x coordinate the Entity will be created at.</param>
        /// <param name="y">The y coordinate the Entity will be created at.</param>
        /// <param name="targetX">The x coordinate the Bullet is travelling towards.</param>
        /// <param name="targetY">The y coordinate the Bullet is travelling towards.</param>
        /// <param name="speed">The speed of the Bullet.</param>
        public static void CreateFreezingBullet(float x, float y, float targetX, float targetY, int speed)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateFreezingBulletImg()));
            components.Add(new CPosition(x, y, 15));
            components.Add(new CFreezingBullet());

            float xOffset = (targetX - x);
            float yOffset = (targetY - y);

            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            float xVel = (xOffset / vectorLength) * speed;
            float yVel = (yOffset / vectorLength) * speed;

            components.Add(new CVelocity(xVel, yVel, speed));
            components.Add(new CProjectile(new CPosition(targetX - 5, targetY - 5, 10, 10)));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components of a Freeze Zone and adds it to the World.
        /// This Entity represents the Freeze Zone created when a Freezing Bullet reaches its target.
        /// </summary>
        /// <param name="pos">The position the Freeze Zone occupies.</param>
        public static void CreateFreezeZone(CPosition pos)
        {
            /// <summary>
            /// Creates a new Entity with just an Animation component to be processed by the Animation Rendering System.
            /// </summary>
            CreateAnimation(pos.X - 50, 
                            pos.Y - 50, 
                            SwinGame.CreateAnimation("Splash", SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim")), 
                            SwinGame.BitmapNamed("FreezingBulletSplash"), SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim"));

            //Create Entity and add to world.
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(pos.Centre.X - 50, pos.Centre.Y - 50, 100));
            components.Add(new CCollidable());
            components.Add(new CPlayerTeam());
            components.Add(new CAppliesDebuff());
            components.Add(new CFrozen(3000, 0));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all the Components of a Poison Zone and adds it to the World.
        /// This Entity represents the Poison Zone the Player can create in the game.
        /// </summary>
        /// <param name="x">The x coordinate the Entity will be created at.</param>
        /// <param name="y">The y coordinate the Entity will be created at.</param>
        public static void CreatePoisonZone(float x, float y)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x - 50, y - 50, 100));
            components.Add(new CPoison(1, 5000, _world.GameTime));
            components.Add(new CAppliesDebuff());
            components.Add(new CPlayerTeam());
            components.Add(new CCollidable());

            Bitmap bmp = SwinGame.BitmapNamed("PoisonZone");
            Animation anim = SwinGame.CreateAnimation("Poison", SwinGame.AnimationScriptNamed("PoisonZoneAnim"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("PoisonZoneAnim");

            components.Add(new CAnimation(bmp, anim, animScript));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with an Animation component and adds it to the world. 
        /// This is primarily used for Entities which are just animations so they can be processed
        /// by the Animation Rendering System.
        /// </summary>
        /// <param name="x">The x coordinate the Entity will be created at.</param>
        /// <param name="y">The y coordinate the Entity will be created at.</param>
        /// <param name="anim">The name of the animation to be played.</param>
        /// <param name="img">The Bitmap the animation will operate on.</param>
        /// <param name="animScript">The animation script containing the animation details.</param>
        public static void CreateAnimation(float x, float y, Animation anim, Bitmap img, AnimationScript animScript)
        {
            //Create Entity and add to world
            Entity newEntity = _world.CreateEntity();

            int width = SwinGame.BitmapWidth(img);
            int height = SwinGame.BitmapHeight(img);

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, width, height));
            components.Add(new CAnimation(img, anim, animScript));

            _world.AddEntity(newEntity, components);
        }
    }
}