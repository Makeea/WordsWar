using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsWar.Multiplayer
{
    public delegate void SnapshotReceivedDelegate(Snapshot snapshot);

    public static class GameClient
    {

        static GameClient()
        {
            PhotonNetwork.OnEventCall += OnPhotonEventCall;
        }
        
        #region GAME MANAGEMENT ===============================================
        public static event SnapshotReceivedDelegate OnSnapshotReceived;
        #endregion

        #region SEND COMMANDS =================================================
        public static void SendCommandPhotonEvent(Command c)
        {
            RaiseEventOptions options = new RaiseEventOptions();
            options.Receivers = ExitGames.Client.Photon.ReceiverGroup.MasterClient;

            PhotonNetwork.RaiseEvent((byte) c.CommandType, c.CommandData, true, options);
        }
        #endregion

        #region RECEIVE SNAPSHOTS==============================================
        private static void OnPhotonEventCall(byte eventCode, object content, int senderId)
        {
            switch (eventCode)
            {
                case Snapshot.SnapshotEventID:
                    BroadcastSnapshotReceivedEvent(Snapshot.FromData((object[])content));
                    break;
            }

        }

        public static void BroadcastSnapshotReceivedEvent(Snapshot e)
        {
            if (OnSnapshotReceived != null)
            {
                OnSnapshotReceived(e);
            }
        }
        #endregion

    }
}
