using System;
namespace MyGame
{
    /// <summary>
    /// Represents a Tag Component indicating that the Entity can apply debuffs.
    /// This is used to distinguish Entities which HAVE a debuff from Entities which INFLICT a debuff. 
    /// </summary>
    public class CAppliesDebuff : Component
    {
    }
}