using System;
namespace MyGame
{
    public class CTeam : Component
    {
        private Team _team;

        public CTeam (Team team)
        {
            _team = team;
        }

        public Team Team
        {
            get {return _team;}
        }
    }
}