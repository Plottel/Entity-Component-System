using System;
namespace MyGame
{
    public class CLoot : Component
    {
        private uint _value;

        public CLoot (uint value)
        {
            _value = value;
        }

        public uint Value
        {
            get {return _value;}
        }
    }
}