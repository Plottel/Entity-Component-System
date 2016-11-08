using System;

namespace MyGame
{
    /// <summary>
    /// Represents a Tag Component indicating that the Entity deals damage on impact.
    /// This is used to unify collision systems and allow multiple Entity types to deal damage when there is a collision.
    /// </summary>
    public class CDamagesOnImpact : Component
    {
        /// <summary>
        /// Represents whether or not the Entity will persist after dealing damage on impact.
        /// </summary>
        private bool _diesAfterImpact;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CDamagesOnImpact"/> class.
        /// </summary>
        /// <param name="diesAfterImpact">Specifies if the Entity will persist after dealing damage on impact.</param>
        public CDamagesOnImpact(bool diesAfterImpact)
        {
            _diesAfterImpact = diesAfterImpact;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MyGame.CDamagesOnImpact"/> dies after impact.
        /// </summary>
        /// <value><c>true</c> if the Entity dies after impact; otherwise, <c>false</c>.</value>
        public bool DiesAfterImpact
        {
            get {return _diesAfterImpact;}
        }
    }
}