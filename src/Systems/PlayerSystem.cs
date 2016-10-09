using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PlayerSystem : System
    {
        private uint _walkerCost;
        private uint _shooterCost;

        public PlayerSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {   
            _walkerCost = 5;
            _shooterCost = 10;
        }

        public void GiveLoot(CLoot lootComp)
        {
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            player.Gold += lootComp.Value;
        }

        public void SpawnWalker()
        {
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            if (player.Gold >= _walkerCost)
            {
                EntityFactory.CreateWalker(150, 300);
                player.Gold -= _walkerCost;
            }
        }

        public void SpawnShooter()
        {
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            if (player.Gold >= _shooterCost)
            {
                
                EntityFactory.CreateShooter(150, 300);
                player.Gold -= _shooterCost;
            }
        }

        public override void Process()
        {
            
        }
    }
}