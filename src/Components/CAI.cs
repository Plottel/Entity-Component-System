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
        /// The attack range.
        /// </summary>
        private int _range;

        /// <summary>
        /// The attack cooldown.
        /// </summary>
        private int _cooldown;

        /// <summary>
        /// The Entity's target.
        /// </summary>
        public ulong TargetID {get; set;}

        /// <summary>
        /// Indicates if the Entity has a target.
        /// </summary>
        /// <returns><c>true</c> if the Entity has a target, <c>false</c> otherwise.</returns>
        public bool HasTarget {get; set;}

        /// <summary>
        /// The last time the Entity attacked. This is used to evaluate if an attack can be performed.
        /// </summary>
        public uint LastAttackTime {get; set;}

        /// <summary>
        /// The attack type of the Entity. This is used to determine what happens when an attack is performed.
        /// </summary>
        private AttackType _attackType;

        /// <summary>
        /// Indicates if the Entity is in range of its target.
        /// </summary>
        public bool IsInRange {get; set;}

        /// <summary>
        /// Indicates if the attack is ready to be used.
        /// </summary>
        /// <returns><c>true</c> if the Entity's attack is ready, <c>false</c> otherwise.</returns>
        public bool AttackIsReady {get; set;}      

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
        public CAI(int range, int cooldown, AttackType attackType, ulong targetID)
        {
            _range = range;
            _cooldown = cooldown;
            _attackType = attackType;
            IsInRange = false;
            AttackIsReady = false;

            /// <summary>
            /// Entity IDs start at 1. If the targetID is 0, the AI is not created with a target.
            /// </summary>
            HasTarget= targetID != 0;
            TargetID = targetID;
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
        /// Gets the type of the attack.
        /// </summary>
        /// <value>The type of the attack.</value>
        public AttackType AttackType
        {
            get {return _attackType;}
        }
    }
}