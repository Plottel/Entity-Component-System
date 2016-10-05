using System;
namespace MyGame
{
    public static class Utils
    {
        public static bool EffectHasEnded(uint gameTime, uint timeApplied, int duration)
        {
            return (gameTime - timeApplied >= duration);
        }
    }
}