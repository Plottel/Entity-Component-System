using System;
using System.Collections.Generic;

namespace MyGame
{
    public interface EntityListener
    {
        void Add(int entID);
        void Remove(int entID);
    }
}