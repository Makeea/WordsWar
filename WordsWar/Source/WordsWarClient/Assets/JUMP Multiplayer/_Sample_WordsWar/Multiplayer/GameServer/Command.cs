using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsWar.Multiplayer
{
    public enum CommandType : byte
    {
        // the byte will be used as Photon command ID, they start at 0 and go up - see 
        // https://doc-api.photonengine.com/en/pun/current/class_exit_games_1_1_client_1_1_photon_1_1_event_code.html
        Connect = 1,
        WordFound = 2,
        Disconnect = 3
    } 

    public abstract class Command
    {
        public CommandType CommandType;
        public object CommandData { get { return data; } }

        protected object[] data;

        internal static Command FromPhotonEvent(byte eventCode, object content)
        {
            switch (eventCode)
            {
                case (byte)CommandType.Connect:
                    return new CommandConnect((int)((object[])content)[0]);
                case (byte)CommandType.WordFound:
                    return new CommandWordFound((string)((object[])content)[0]);
                default:
                    return null;
            }
        }
    }

    public class CommandConnect : Command
    {
        public int PlayerId { get { return (int)data[0]; } }

        public CommandConnect(int playerID)
        {
            CommandType = CommandType.Connect;
            data = new object[1];

            data[0] = playerID;
        }
    }

    public class CommandWordFound : Command
    {
        public string Word { get { return (string)data[0]; } }

        public CommandWordFound(string word)
        {
            CommandType = CommandType.WordFound;
            data = new object[1];

            data[0] = word;
        }
    }

}
