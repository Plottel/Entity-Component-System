using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PlayerGoldSystem : System
    {
        private uint _archerCost;
        private uint _wizardCost;
        private int _minimumFreezingBulletCooldown = 200;
        private int _minimumPoisonZoneCooldown = 2000;
        private Random _spawnAt = new Random();

        public PlayerGoldSystem(World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {   
            _archerCost = 10;
            _wizardCost = 20;
        }

        public void GiveLoot(CLoot lootComp)
        {
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            player.Gold += lootComp.Value;
        }

        public void BuyWizard(CPlayer player)
        {
            if (player.Gold > _wizardCost)
            {
                player.WizardCount += 1;
                player.Gold -= _wizardCost;

                if (player.FreezingBulletCooldown > _minimumFreezingBulletCooldown)
                {
                    player.FreezingBulletCooldown -= 20;
                }

                if (player.PoisonZoneCooldown > _minimumPoisonZoneCooldown)
                {
                    player.PoisonZoneCooldown -= 20;
                }
            }
        }

        public void BuyArcher(CPlayer player)
        {
            if (player.Gold >= _archerCost)
            {
                EntityFactory.CreatePlayerArcher(50, _spawnAt.Next(20, 550));
                player.Gold -= _archerCost;
                player.ArcherCount += 1;
            }
        }

        public override void Process()
        {
            
        }
    }
}