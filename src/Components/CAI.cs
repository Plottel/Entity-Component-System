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
        private AIState _state;
        private bool _isInRange;

        public CAI (int range, int cooldown, AttackType attackType)
        {
            _range = range;
            _cooldown = cooldown;
            _attackType = attackType;
            _state = AIState.GetTarget;
            _isInRange = false;
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

        public AIState State
        {
            get {return _state;}
            set {_state = value;}
        }
    }
}