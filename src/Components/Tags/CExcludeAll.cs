using System;
namespace MyGame
{
    /// <summary>
    /// Represents a Tag Component indicating that this System will not accept any Entities into its List.
    /// No Entities should have this Component - it should exist only in a System's Exclusion Mask.
    /// </summary>
    public class CExcludeAll : Component
    {
    }
}