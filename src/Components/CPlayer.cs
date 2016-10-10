using System;
namespace MyGame
{
    public class CPlayer: Component
    {
        private uint _gold;
        private uint _freezingBulletCooldown;
        private uint _usedLastFreezingBulletAt;
        private uint _poisonZoneCooldown;
        private uint _usedlastPoisonZoneAt;
        private int _archerCount;
        private int _wizardCount;

        public CPlayer()
        {
            _gold = 5000;
            _freezingBulletCooldown = 1000;
            _poisonZoneCooldown = 5000;
            _archerCount = 0;
            _wizardCount = 0;
        }

        public uint FreezingBulletCooldown
        {
            get {return _freezingBulletCooldown;}
            set {_freezingBulletCooldown = value;}
        }

        public uint PoisonZoneCooldown
        {
            get {return _poisonZoneCooldown;}
            set {_poisonZoneCooldown = value;}
        }

        public uint Gold
        {
            get {return _gold;}
            set {_gold = value;}
        }

        public int ArcherCount
        {
            get {return _archerCount;}
            set {_archerCount = value;}
        }

        public int WizardCount
        {
            get {return _wizardCount;}
            set {_wizardCount = value;}
        }

        public uint UsedLastFreezingBulletAt
        {
            get {return _usedLastFreezingBulletAt;}
            set {_usedLastFreezingBulletAt = value;}
        }

        public uint UsedLastPoisonZoneAt
        {
            get {return _usedlastPoisonZoneAt;}
            set {_usedlastPoisonZoneAt = value;}
        }
    }
}