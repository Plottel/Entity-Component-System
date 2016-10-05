using System;
namespace MyGame
{
    //ComponentType class
    //Contains a dictionary mapping ints (masks) against Types
    public enum ComponentType
    {
        None = 1 << 0,
        Position = 1 << 1,
        Renderable = 1 << 2,
        Velocity = 1 << 3,
        Player = 1 << 4,
        Health = 1 << 5,
        AI = 1 << 6,
        Projectile = 1 << 7,
        Gun = 1 << 8,
        Damage = 1 << 9,
        NotMoving = 1 << 10,
        Poison = 1 << 11,
        ApplyPoison = 1 << 12,
    }
}