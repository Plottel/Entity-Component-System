using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the Artificial Intelligence Component. Entities which Attack will have this Component. 
    /// Each AI has a Target which they are trying to attack.
    /// </summary>
    public class CAI: Component
    {
        /// <summary>
        /// The target.
        /// </summary>
        private int _targetID;

        /// <summary>
        /// Indicates if the Entity has a target.
        /// </summary>
        private bool _hasTarget;

        /// <summary>
        /// The attack range.
        /// </summary>
        private int _range;

        /// <summary>
        /// The attack cooldown.
        /// </summary>
        private int _cooldown;

        /// <summary>
        /// The last time the Entity attacked. This is used to evaluate if an attack can be performed.
        /// </summary>
        private uint _lastAttackTime;

        /// <summary>
        /// The attack type of the Entity. This is used to determine what happens when an attack is performed.
        /// </summary>
        private AttackType _attackType;

        /// <summary>
        /// Indicates if the Entity is in range of its target.
        /// </summary>
        private bool _isInRange;

        /// <summary>
        /// Indicates if the attack is ready to be used.
        /// </summary>
        private bool _attackIsReady;      

        /// <summary>
        /// An AI Component can be created with or without a TargetID.
        /// </summary>
        public CAI (int range, int cooldown, AttackType attackType) : this (range, cooldown, attackType, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CAI"/> class.
        /// </summary>
        /// <param name="range">The attack range.</param>
        /// <param name="cooldown">Cooldown.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="targetID">Target identifier.</param>
        public CAI(int range, int cooldown, AttackType attackType, int targetID)
        {
            _range = range;
            _cooldown = cooldown;
            _attackType = attackType;
            _isInRange = false;
            _attackIsReady = false;

            /// <summary>
            /// Entity IDs start at 1. If the targetID is 0, the AI is not created with a target.
            /// </summary>
            _hasTarget = targetID != 0;
            _targetID = targetID;
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public int TargetID
        {
            get {return _targetID;}
            set {_targetID = value;}
        }

        /// <summary>
        /// Gets the attack range.
        /// </summary>
        /// <value>The attack range.</value>
        public int Range
        {
            get {return _range;}
        }

        /// <summary>
        /// Gets the attack cooldown.
        /// </summary>
        /// <value>The attack cooldown.</value>
        public int Cooldown
        {
            get {return _cooldown;}
        }

        /// <summary>
        /// Gets or sets the time the last attack was performed.
        /// </summary>
        /// <value>The last attack time.</value>
        public uint LastAttackTime
        {
            get {return _lastAttackTime;}
            set {_lastAttackTime = value;}
        }

        /// <summary>
        /// Gets the type of the attack.
        /// </summary>
        /// <value>The type of the attack.</value>
        public AttackType AttackType
        {
            get {return _attackType;}
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Entity is in range to attack.
        /// </summary>
        /// <value><c>true</c> if is in range; otherwise, <c>false</c>.</value>
        public bool IsInRange
        {
            get {return _isInRange;}
            set {_isInRange = value;}
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Entity's attack is ready.
        /// </summary>
        /// <value><c>true</c> if attack is ready; otherwise, <c>false</c>.</value>
        public bool AttackIsReady
        {
            get {return _attackIsReady;}
            set {_attackIsReady = value;}
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Entity has a valid target.
        /// </summary>
        /// <value><c>true</c> if has target; otherwise, <c>false</c>.</value>
        public bool HasTarget
        {
            get {return _hasTarget;}
            set {_hasTarget = value;}
        }
    }
}