using System;
using SwinGameSDK;

namespace MyGame
{
    public class CAI: Component
    {
        private int _targetID;
        private int _range;
        private int _cooldown;
        private uint _lastAttackTime;
        private AttackType _attackType;
        private bool _isInRange;
        private bool _attackIsReady;
        private bool _hasTarget;

        public CAI (int range, int cooldown, AttackType attackType)
        {
            _range = range;
            _cooldown = cooldown;
            _attackType = attackType;
            _isInRange = false;
            _attackIsReady = false;
            _hasTarget = false;
        }

        public CAI(int range, int cooldown, AttackType attackType, int targetID)
        {
            _range = range;
            _cooldown = cooldown;
            _attackType = attackType;
            _isInRange = false;
            _attackIsReady = false;
            _hasTarget = true;
            _targetID = targetID;
        }

        public int TargetID
        {
            get {return _targetID;}
            set {_targetID = value;}
        }

        public int Range
        {
            get {return _range;}
            set {_range = value;}
        }

        public int Cooldown
        {
            get {return _cooldown;}
            set {_cooldown = value;}
        }

        public uint LastAttackTime
        {
            get {return _lastAttackTime;}
            set {_lastAttackTime = value;}
        }

        public AttackType AttackType
        {
            get {return _attackType;}
        }

        public bool IsInRange
        {
            get {return _isInRange;}
            set {_isInRange = value;}
        }

        public bool AttackIsReady
        {
            get {return _attackIsReady;}
            set {_attackIsReady = value;}
        }

        public bool HasTarget
        {
            get {return _hasTarget;}
            set {_hasTarget = value;}
        }
    }
}