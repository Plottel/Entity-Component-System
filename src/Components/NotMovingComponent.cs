using System;
namespace MyGame
{
    public class NotMovingComponent : Component
    {
        private int _duration;
        private uint _timeApplied;

        public NotMovingComponent (int duration, uint timeApplied)
        {
            _duration = duration;
            _timeApplied = timeApplied;
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