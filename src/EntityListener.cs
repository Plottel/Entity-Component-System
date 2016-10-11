using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the interface implemented by all Systems. This allows Systems to subscribe
    /// to the World and add / remove Entities from their lists whenever an Entity is 
    /// added or removed from the World. This keeps the master list of Entities in sync across all Systems.
    /// </summary>
    public interface EntityListener
    {
        void Add(int entID);
        void Remove(int entID);
    }
}