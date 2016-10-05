using System;
namespace MyGame
{
    public class PoisonComponent : Component
    {
        private int _strength;
        private int _duration;
        private uint _timeApplied;

        public PoisonComponent (int strength, int duration, uint timeApplied)
        {
            _strength = strength;
            _duration = duration;
            _timeApplied = timeApplied;
        }

        public int Strength
        {
            get {return _strength;}
        }

        public int Duration
        {
            get {return _duration;}
        }

        public uint TimeApplied
        {
            get {return _timeApplied;}
            set {_timeApplied = value;}
        }
    }
}