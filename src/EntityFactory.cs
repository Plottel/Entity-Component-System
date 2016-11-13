using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the class where all Entities are created. As Entities are just integers, this class and its methods
    /// provide templates for creating different Entity "types".
    /// </summary>
    public static class EntityFactory
    {
        #region DECLARE ENTITY SIZE AND POSITION DETAILS

        public const int CASTLE_X = 0;
        public const int CASTLE_Y = 5;
        public const int CASTLE_WIDTH = 100;
        public const int CASTLE_HEIGHT = 590;

        public const int SWORD_MAN_WIDTH = 21;
        public const int SWORD_MAN_HEIGHT = 30;

        public const int ARCHER_WIDTH = 20;
        public const int ARCHER_HEIGHT = 28;

        public const int BATTERING_RAM_WIDTH = 64;
        public const int BATTERING_RAM_HEIGHT = 58;

        public const int ARROW_SIZE = 15;

        public const int FREEZING_BULLET_SIZE = 15;

        public const int EXPLOSION_SIZE = 120;

        public const int EXPLOSION_MAN_SIZE = 34;

        public const int FREEZE_ZONE_SIZE = 100;

        public const int POISON_ZONE_SIZE = 100;

        #endregion DECLARE ENTITY SIZE AND POSITION DETAILS

        #region DECLARE ENTITY COMBAT DETAILS
        public const int CASTLE_HP = 5000;

        public const int SWORD_MAN_HP = 20;
        public const int SWORD_MAN_SPEED = 2;
        public const int SWORD_MAN_DAMAGE = 1;
        public const int SWORD_MAN_RANGE = 1;
        public const int SWORD_MAN_COOLDOWN = 1000;
        public const int SWORD_MAN_LOOT_VALUE = 5;

        public const int PLAYER_ARROW_SPEED = 25;
        public const int PLAYER_ARROW_DAMAGE = 2;

        public const int PLAYER_ARCHER_RANGE = 700;
        public const int PLAYER_ARCHER_COOLDOWN = 1000;

        public const int ENEMY_ARCHER_HP = 10;
        public const int ENEMY_ARCHER_SPEED = 1;
        public const int ENEMY_ARCHER_RANGE = 100;
        public const int ENEMY_ARCHER_COOLDOWN = 2000;
        public const int ENEMY_ARCHER_LOOT_VALUE = 10;

        public const int ENEMY_ARROW_SPEED = 5;
        public const int ENEMY_ARROW_DAMAGE = 2;

        public const int BATTERING_RAM_HP = 50;
        public const int BATTERING_RAM_SPEED = 1;
        public const int BATTERING_RAM_DAMAGE = 5;
        public const int BATTERING_RAM_RANGE = 1;
        public const int BATTERING_RAM_COOLDOWN = 5000;
        public const int BATTERING_RAM_LOOT_VALUE = 50;

        public const int EXPLOSION_DAMAGE = 500;

        public const int FREEZING_BULLET_SPEED = 7;
        public const int FREEZING_BULLET_FREEZE_DURATION = 3000;

        public const int POISON_ZONE_STRENGTH = 1;
        public const int POISON_ZONE_POISON_DURATION = 5000;
        public const int POISON_ZONE_LIFETIME = 5000;
        #endregion DECLARE ENTITY COMBAT DETAILS

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
        /// <returns>The Castle Bitmap.</returns>
        public static Bitmap CreateCastleImg()
        {
            Bitmap result = SwinGame.CreateBitmap(CASTLE_WIDTH, CASTLE_HEIGHT);
            SwinGame.ClearSurface(result, SwinGame.RGBAColor(50, 50, 40, 140));

            return result;
        }

        /// <summary>
        /// Returns a Bitmap representing a Freezing Bullet.
        /// </summary>
        /// <returns>The freezing bullet Bitmap.</returns>
        public static Bitmap CreateFreezingBulletImg()
        {
            Bitmap result = SwinGame.CreateBitmap(FREEZING_BULLET_SIZE, FREEZING_BULLET_SIZE);
            SwinGame.ClearSurface(result, Color.DarkTurquoise);

            return result;
        }

        /// <summary>
        /// Creates an Entity with all Components of a Player and adds it to the World.
        /// </summary>
        public static void CreatePlayer()
        {
            //Create Entity and add to world
            ulong newEntity = _world.NextEntityID;
            PlayerSystem.PLAYER_ENTITY_ID = newEntity;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPlayerTeam());
            components.Add(new CRenderable(CreateCastleImg()));
            components.Add(new CPosition(CASTLE_X, CASTLE_Y, CASTLE_WIDTH, CASTLE_HEIGHT));
            components.Add(new CPlayer());
            components.Add(new CHealth(CASTLE_HP));
            components.Add(new CCollidable());

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
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CEnemyTeam());
            components.Add(new CStatusAnimations());
            components.Add(new CPosition(x, y, SWORD_MAN_WIDTH, SWORD_MAN_HEIGHT));
            components.Add(new CVelocity(-SWORD_MAN_SPEED, 0, SWORD_MAN_SPEED)); //Is created moving towards the Castle.
            components.Add(new CHealth(SWORD_MAN_HP));
            components.Add(new CAI(SWORD_MAN_RANGE, SWORD_MAN_COOLDOWN, AttackType.Melee, PlayerSystem.PLAYER_ENTITY_ID)); //Sword Men are on Enemy team and target the Castle by default
            components.Add(new CDamage(SWORD_MAN_DAMAGE));
            components.Add(new CLoot(SWORD_MAN_LOOT_VALUE));
            components.Add(new CCollidable());

            /// <summary>
            /// Details for Animation Component.
            /// </summary>
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
            //Create Entity and add to world
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPlayerTeam());
            components.Add(new CPosition(x, y, ARCHER_WIDTH, ARCHER_HEIGHT));
            components.Add(new CAI(PLAYER_ARCHER_RANGE, PLAYER_ARCHER_COOLDOWN, AttackType.Bow));
            components.Add(new CBow(PLAYER_ARROW_SPEED, PLAYER_ARROW_DAMAGE));

            /// <summary>
            /// Details for Animation Component.
            /// </summary>
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
            //Create Entity and add to World.
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CEnemyTeam());
            components.Add(new CStatusAnimations());
            components.Add(new CPosition(x, y, ARCHER_WIDTH, ARCHER_HEIGHT));
            components.Add(new CVelocity(-ENEMY_ARCHER_SPEED, 0, ENEMY_ARCHER_SPEED)); //Is created moving towards the castle.
            components.Add(new CHealth(ENEMY_ARCHER_HP));
            components.Add(new CAI(ENEMY_ARCHER_RANGE, ENEMY_ARCHER_COOLDOWN, AttackType.Bow, PlayerSystem.PLAYER_ENTITY_ID)); //Belongs to the Enemy team - attacks the Castle by default.
            components.Add(new CBow(ENEMY_ARROW_SPEED, ENEMY_ARROW_DAMAGE));
            components.Add(new CLoot(ENEMY_ARCHER_LOOT_VALUE));
            components.Add(new CCollidable());

            /// <summary>
            /// Details for Animation Component.
            /// </summary>
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
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CEnemyTeam());
            components.Add(new CStatusAnimations());
            components.Add(new CPosition(x, y, BATTERING_RAM_WIDTH, BATTERING_RAM_HEIGHT));
            components.Add(new CVelocity(-BATTERING_RAM_SPEED, 0, BATTERING_RAM_SPEED));
            components.Add(new CHealth(BATTERING_RAM_HP));
            components.Add(new CAI(BATTERING_RAM_RANGE, BATTERING_RAM_COOLDOWN, AttackType.Melee, PlayerSystem.PLAYER_ENTITY_ID)); //Belongs to the Enemy team - targets the Castle by default
            components.Add(new CDamage(BATTERING_RAM_DAMAGE));
            components.Add(new CLoot(BATTERING_RAM_LOOT_VALUE));
            components.Add(new CCollidable());

            /// <summary>
            /// Details for Animation Component.
            /// </summary>
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
        /// <param name="bow">The bow determining speed and damage of the Arrow.</param>
        /// <param name="targetPos">The position of the Entity the Arrow is heading towards.</param>
        /// <typeparam name="T">The team the Arrow belongs to</typeparam>
        public static void CreateArrow<T>(float x, float y, CBow bow, CPosition targetPos) where T : CTeam
        {
            //Create Entity and add to world
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPosition(x, y, ARROW_SIZE));
            components.Add(new CDamage(bow.ArrowDamage));
            components.Add(new CCollidable());
            components.Add(new CProjectile());
            components.Add(new CDamagesOnImpact(true));

            //Calculate the centre of the passed in position to be the Arrow's target.
            float targetCentreX = targetPos.X + (targetPos.Width / 2);
            float targetCentreY = targetPos.Y + (targetPos.Height / 2);

            //Calculate appropriate velocities for Arrow to reach target.
            Vector velocity = Utils.GetVectorBetweenPoints(x, y, targetCentreX, targetCentreY, bow.ArrowSpeed);
            components.Add(new CVelocity(velocity.X, velocity.Y, bow.ArrowSpeed));

            if (typeof(T) == typeof(CPlayerTeam))
            {
                components.Add(new CPlayerTeam());
                components.Add(new CRenderable(SwinGame.BitmapNamed("PlayerArrow")));                
            }
            else if (typeof(T) == typeof(CEnemyTeam))
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
        public static void CreateFreezingBullet(float x, float y, float targetX, float targetY)
        {
            //Create Entity and add to world
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CRenderable(CreateFreezingBulletImg()));
            components.Add(new CPosition(x, y, FREEZING_BULLET_SIZE));
            components.Add(new CFreezingBullet(targetX + (FREEZING_BULLET_SIZE / 2), targetY + (FREEZING_BULLET_SIZE / 2)));
            components.Add(new CProjectile());

            //Calculate velocity to target and add Velocity Component.
            Vector velocity = Utils.GetVectorBetweenPoints(x, y, targetX, targetY, FREEZING_BULLET_SPEED);
            components.Add(new CVelocity(velocity.X, velocity.Y, FREEZING_BULLET_SPEED));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components for an Explosion Man and adds it to the World.
        /// This Entity represents the exploding wizard the Player can purchase.
        /// </summary>
        public static void CreateExplosionMan()
        {
            //Create Entity and add to world
            ulong newEntity = _world.NextEntityID;

            /// <summary>
            /// Identifies the most populated Enemy Spatial Hash Cell. The Explosion man
            /// will be spawned at the centre of this cell.
            /// </summary>
            CollisionCheckSystem collChkSys = World.GetSystem<CollisionCheckSystem>();
            int mostPopulatedCell = collChkSys.EnemyCells.Aggregate((l, r) => l.Value.Count > r.Value.Count ? l : r).Key;
            Point2D pos = collChkSys.CentreOfCell(mostPopulatedCell);

            /// <summary>
            /// Adjust position for Sprite size so Explosion Man spawns at centre.
            /// </summary>
            float atX = pos.X - (EXPLOSION_MAN_SIZE / 2);
            float atY = pos.Y - (EXPLOSION_MAN_SIZE / 2);

            /// <summary>
            /// Animation details for Animation component.
            /// </summary>
            Bitmap bmp = SwinGame.BitmapNamed("ExplosionMan");
            Animation anim = SwinGame.CreateAnimation("Spawn", SwinGame.AnimationScriptNamed("ExplosionAnim"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("ExplosionAnim");

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPlayerTeam());
            components.Add(new CPosition(atX, atY, EXPLOSION_MAN_SIZE));
            components.Add(new CAnimation(bmp, anim, animScript));
            components.Add(new CExplosionMan(mostPopulatedCell));

            _world.AddEntity(newEntity, components);
        }

        /// <summary>
        /// Creates an Entity with all Components of a Freeze Zone and adds it to the World.
        /// This Entity represents the Freeze Zone created when a Freezing Bullet reaches its target.
        /// </summary>
        /// <param name="pos">The position the Freeze Zone occupies.</param>
        public static void CreateFreezeZone(CPosition pos)
        {
            //Create Entity and add to world.
            ulong newEntity = _world.NextEntityID;

            /// <summary>
            /// Adjust position for Sprite size so Freeze Zone spawns at centre of bullet.
            /// </summary>
            float atX = pos.X - (FREEZE_ZONE_SIZE / 2);
            float atY = pos.Y - (FREEZE_ZONE_SIZE / 2);

            /// <summary>
            /// Creates a new Entity with just an Animation component to be processed by the Animation Rendering System.
            /// </summary>
            Bitmap bmp = SwinGame.BitmapNamed("FreezingBulletSplash");
            Animation anim = SwinGame.CreateAnimation("Splash", SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim"));
            AnimationScript animScript = SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim");

            CreateAnimationEntity(atX, atY, anim, bmp, animScript);

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPlayerTeam());
            components.Add(new CPosition(pos.Centre.X - (FREEZE_ZONE_SIZE / 2), pos.Centre.Y - (FREEZE_ZONE_SIZE / 2), FREEZE_ZONE_SIZE));
            components.Add(new CCollidable());
            components.Add(new CAppliesDebuff());
            components.Add(new CFrozen(FREEZING_BULLET_FREEZE_DURATION, 0));

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
            ulong newEntity = _world.NextEntityID;

            //Create components and pass to world to send to Systems
            List<Component> components = new List<Component>();
            components.Add(new CPlayerTeam());
            components.Add(new CPosition(x - (POISON_ZONE_SIZE / 2), y - (POISON_ZONE_SIZE / 2), POISON_ZONE_SIZE));
            components.Add(new CPoison(POISON_ZONE_STRENGTH, POISON_ZONE_POISON_DURATION, 0));
            components.Add(new CLifetime(POISON_ZONE_LIFETIME));
            components.Add(new CAppliesDebuff());
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
        public static void CreateAnimationEntity(float x, float y, Animation anim, Bitmap img, AnimationScript animScript)
        {
            //Create Entity and add to world
            ulong newEntity = _world.NextEntityID;

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