using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsWar.Multiplayer
{
    public class Snapshot
    {
        public const byte SnapshotEventID = 0;

        private const int NumProperties = 5;
        private object[] data = new object[NumProperties];

        public int PlayerID                 { get { return (int) data[0]; } set { data[0] = value; } }
        public GameServerStates GameState   { get { return (GameServerStates) data[1]; } set { data[1] = value; } }
        public double SecondsLeft         { get { return (double) data[2]; } set { data[2] = value; } }
        public int Player1Score             { get { return (int) data[3]; } set { data[3] = value; } }
        public int Player2Score             { get { return (int) data[4]; } set { data[4] = value; } }

        public object CommandData { get { return data; } }

        public static Snapshot FromData(object[] data)
        {
            if ((data == null) || (data.Length != NumProperties))
            {
                throw new ArgumentException("Data must be not null and an array with " + NumProperties.ToString() + " elements");
            }

            Snapshot res = new Snapshot();
            res.data = data;
            return res;
        }

    }
}
