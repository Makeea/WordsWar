using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsWar.Multiplayer
{
    public static class GameClient
    {

        public static void Start()
        {

        }

        public static void SendCommand(Command c)
        {
            RaiseEventOptions options = new RaiseEventOptions();
            options.Receivers = ExitGames.Client.Photon.ReceiverGroup.MasterClient;

            PhotonNetwork.RaiseEvent((byte) c.CommandType, c.CommandData, true, options);
        }

        public static void ReceiveSnapshot(Snapshot s)
        {

        }
    }
}
