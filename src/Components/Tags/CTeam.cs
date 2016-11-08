using System;
namespace MyGame
{
    /// <summary>
    /// Represents the Component indicating that the Entity belongs to a team.
    /// Components inheriting from this class will indicate specific teams. This approach is preferred
    /// to having a single Team Component containing a Team value as it allows team to be ascertained 
    /// purely with World.EntityHasComponent rather than the 2-step process of fetching the Team
    /// Component and reading the value.
    /// </summary>
    public class CTeam : Component
    {
        public CTeam ()
        {
        }
    }
}
