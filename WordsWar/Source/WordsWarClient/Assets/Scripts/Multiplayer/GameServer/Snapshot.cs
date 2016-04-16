using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsWar.Multiplayer
{
    public class Snapshot
    {
        public int PlayerID;
        public GameServerStates GameState;
        public TimeSpan SecondsLeft;
        public int Player1Score;
        public int Player2Score;
    }
}
