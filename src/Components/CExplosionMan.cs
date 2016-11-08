using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Explosion Man Component. This is used to identify an Explosion Man
    /// and to track its state (is it spawning or exploding?).
    /// </summary>
    public class CExplosionMan : Component
    {
        /// <summary>
        /// Indicates whether or not the Explosion Man is ready to explode.
        /// When the spawn animation has finished, the Explosion Man is ready.
        /// </summary>
        private bool _readyToExplode;

        /// <summary>
        /// Represents the cell ID the Explosion Man will explode in the centre of.
        /// This is used for determining position.
        /// </summary>
        private int _targetCell;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CExplosionMan"/> class.
        /// </summary>
        /// <param name="targetCell">The cell the Explosion Man will target.</param>
        public CExplosionMan(int targetCell)
        {
            _targetCell = targetCell;
            _readyToExplode = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Explosion Man is ready to explode.
        /// </summary>
        /// <value><c>true</c> if ready to explode; otherwise, <c>false</c>.</value>
        public bool ReadyToExplode
        {
            get {return _readyToExplode;}
            set {_readyToExplode = value;}
        }

        /// <summary>
        /// Gets the ID of the cell the Explosion Man will target.
        /// </summary>
        /// <value>The target cell.</value>
        public int TargetCell
        {
            get {return _targetCell;}
        }
    }
}